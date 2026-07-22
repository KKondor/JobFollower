import { useState, useEffect } from "react";
import type { JobApplicationDto, CreateJobDto, JobPatchDto } from "../types/job";
import { StatusState } from "../types/job";

interface JobFormModalProps {
    isOpen: boolean;
    onClose: () => void;
    existingJob?: JobApplicationDto;
    onCreate: (job: CreateJobDto) => Promise<void>;
    onUpdate: (id: number, patch: JobPatchDto) => Promise<void>;
    onDelete: (id: number) => Promise<void>;
}

export default function JobFormModal({isOpen, onClose, existingJob,
                                         onCreate, onUpdate, onDelete,}: JobFormModalProps) {
    const [jobName, setJobName] = useState("");
    const [jobDescription, setJobDescription] = useState("");
    const [status, setStatus] = useState<string>(StatusState.NotApplied);

    useEffect(() => {
        if (existingJob) {
            setJobName(existingJob.jobName);
            setJobDescription(existingJob.jobDescription ?? "");
            setStatus(existingJob.status);
        } else {
            setJobName("");
            setJobDescription("");
            setStatus(StatusState.NotApplied);
        }
    }, [existingJob, isOpen]);

    if (!isOpen) return null;

    async function handleSubmit(e: React.SubmitEvent<HTMLFormElement>) {
        e.preventDefault();

        if (existingJob) {
            await onUpdate(existingJob.jobId, {
                jobName,
                jobDescription: jobDescription || undefined,
                status: status as JobApplicationDto["status"],
            });
        } else {
            await onCreate({
                jobName,
                jobDescription: jobDescription || null,
                status: status as JobApplicationDto["status"],
                appliedDate: new Date().toISOString(),
            });
        }

        onClose();
    }

    async function handleDelete() {
        if (!existingJob) return;
        if (!confirm(`Delete "${existingJob.jobName}"? This can't be undone.`)) return;
        await onDelete(existingJob.jobId);
        onClose();
    }

    return (
        <div
            style={{
                position: "fixed",
                inset: 0,
                backgroundColor: "rgba(0,0,0,0.5)",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
            }}
            onClick={onClose}
        >
            <div
                style={{ background: "white", padding: "1.5rem", borderRadius: "8px", minWidth: "320px" }}
                onClick={(e) => e.stopPropagation()}
            >
                <button type="button" onClick={onClose} style={{ float: "right" }}>
                    ×
                </button>
                <h2>{existingJob ? "Edit Job" : "New Job"}</h2>
                <form onSubmit={handleSubmit}>
                    <input
                        type="text"
                        placeholder="Job title"
                        value={jobName}
                        onChange={(e) => setJobName(e.target.value)}
                        required
                    />
                    <input
                        type="text"
                        placeholder="Description (optional)"
                        value={jobDescription}
                        onChange={(e) => setJobDescription(e.target.value)}
                    />
                    <select value={status} onChange={(e) => setStatus(e.target.value)}>
                        {Object.values(StatusState).map((s) => (
                            <option key={s} value={s}>{s}</option>
                        ))}
                    </select>
                    <button type="submit">{existingJob ? "Save" : "Create"}</button>
                    {existingJob && (
                        <button type="button" onClick={handleDelete} style={{ color: "red" }}>
                            Delete
                        </button>
                    )}
                </form>
            </div>
        </div>
    );
}