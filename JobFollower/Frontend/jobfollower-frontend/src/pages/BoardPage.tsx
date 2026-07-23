import { useEffect, useState } from "react";
import { DndContext, DragOverlay, useSensor, useSensors, PointerSensor } from "@dnd-kit/core";
import type { DragEndEvent, DragStartEvent } from "@dnd-kit/core";
import { getJobs, createJob, patchJob, deleteJob } from "../api/jobsApi";
import type { JobApplicationDto, CreateJobDto, JobPatchDto } from "../types/job";
import { StatusState } from "../types/job";
import { useDelayedFlag } from "../utils/useDelayedFlag";
import JobColumn from "../components/JobColumn";
import JobFormModal from "../components/JobFormModal";
import styles from "./BoardPage.module.css"
import jobCardStyles from "../components/JobCard.module.css"
import LoadingScreen from "../components/LoadingScreen";

export default function BoardPage() {
    const [jobs, setJobs] = useState<JobApplicationDto[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingJob, setEditingJob] = useState<JobApplicationDto | undefined>(undefined);
    const [activeJob, setActiveJob] = useState<JobApplicationDto | null>(null);
    const showColdStartMessage = useDelayedFlag(isLoading, 3000);

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

    async function handleDelete(id: number) {
        await deleteJob(id);
        setJobs((prev) => prev.filter((j) => j.jobId !== id));
    }

    function handleDragStart(event: DragStartEvent) {
        const job = jobs.find((j) => j.jobId === event.active.id);
        setActiveJob(job ?? null);
    }

    async function handleDragEnd(event: DragEndEvent) {
        setActiveJob(null);
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

    if (isLoading) {
        return (
            <LoadingScreen message="Loading your board...">
                {showColdStartMessage && (
                    <p className={styles.coldStartMessage}>
                        The server may be waking up from sleep — this can take up to a minute.
                    </p>
                )}
            </LoadingScreen>
        );
    }
    if (error) return <div>{error}</div>;

    const columns = Object.values(StatusState);

    return (
        <div className={styles.page}>
            <div className={styles.header}>
                <h1 className={styles.title}>Job Applications</h1>
                <button className={styles.newJobButton} onClick={openCreateModal}>
                    + New Job
                </button>
            </div>

            <DndContext sensors={sensors} onDragStart={handleDragStart} onDragEnd={handleDragEnd}>
                <div className={styles.board}>
                    {columns.map((status) => (
                        <JobColumn
                            key={status}
                            status={status}
                            jobs={jobs.filter((job) => job.status === status)}
                            onCardClick={openEditModal}
                        />
                    ))}
                </div>
                <DragOverlay>
                    {activeJob ? (
                        <div className={jobCardStyles.card}>
                            <p className={jobCardStyles.jobName}>{activeJob.jobName}</p>
                            {activeJob.jobDescription && <p className={jobCardStyles.jobDescription}>{activeJob.jobDescription}</p>}
                        </div>
                    ) : null}
                </DragOverlay>
            </DndContext>

            <JobFormModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                existingJob={editingJob}
                onCreate={handleCreate}
                onUpdate={handleUpdate}
                onDelete={handleDelete}
            />
        </div>
    );
}