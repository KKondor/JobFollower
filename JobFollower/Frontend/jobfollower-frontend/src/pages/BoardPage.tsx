import { useEffect, useState } from "react";
import { DndContext, useSensor, useSensors, PointerSensor } from "@dnd-kit/core";
import type { DragEndEvent } from "@dnd-kit/core";
import { getJobs, createJob, patchJob } from "../api/jobsApi";
import type { JobApplicationDto, CreateJobDto, JobPatchDto } from "../types/job";
import { StatusState } from "../types/job";
import JobColumn from "../components/JobColumn";
import JobFormModal from "../components/JobFormModal";

export default function BoardPage() {
    const [jobs, setJobs] = useState<JobApplicationDto[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingJob, setEditingJob] = useState<JobApplicationDto | undefined>(undefined);

    const sensors = useSensors(
        useSensor(PointerSensor, {
            activationConstraint: {
                distance: 8, // pixels of movement required before a drag "activates"
            },
        })
    );

    useEffect(() => {
        async function loadJobs() {
            try {
                const data = await getJobs();
                setJobs(data);
            } catch {
                setError("Failed to load job applications.");
            } finally {
                setIsLoading(false);
            }
        }
        loadJobs();
    }, []);

    function openCreateModal() {
        setEditingJob(undefined);
        setIsModalOpen(true);
    }

    function openEditModal(job: JobApplicationDto) {
        setEditingJob(job);
        setIsModalOpen(true);
    }

    async function handleCreate(job: CreateJobDto) {
        const created = await createJob(job);
        setJobs((prev) => [...prev, created]);
    }

    async function handleUpdate(id: number, patch: JobPatchDto) {
        const updated = await patchJob(id, patch);
        setJobs((prev) => prev.map((j) => (j.jobId === id ? updated : j)));
    }

    async function handleDragEnd(event: DragEndEvent) {
        const { active, over } = event;
        if (!over) return;

        const jobId = active.id as number;
        const newStatus = over.id as string;
        const job = jobs.find((j) => j.jobId === jobId);
        if (!job || job.status === newStatus) return;

        setJobs((prev) =>
            prev.map((j) => (j.jobId === jobId ? { ...j, status: newStatus as typeof j.status } : j))
        );

        try {
            await patchJob(jobId, { status: newStatus as typeof job.status });
        } catch {
            setJobs((prev) => prev.map((j) => (j.jobId === jobId ? { ...j, status: job.status } : j)));
        }
    }

    if (isLoading) return <div>Loading board...</div>;
    if (error) return <div>{error}</div>;

    const columns = Object.values(StatusState);

    return (
        <div>
            <button onClick={openCreateModal}>+ New Job</button>

            <DndContext sensors={sensors} onDragEnd={handleDragEnd}>
                <div style={{ display: "flex", gap: "1rem" }}>
                    {columns.map((status) => (
                        <JobColumn
                            key={status}
                            status={status}
                            jobs={jobs.filter((job) => job.status === status)}
                            onCardClick={openEditModal}
                        />
                    ))}
                </div>
            </DndContext>

            <JobFormModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                existingJob={editingJob}
                onCreate={handleCreate}
                onUpdate={handleUpdate}
            />
        </div>
    );
}