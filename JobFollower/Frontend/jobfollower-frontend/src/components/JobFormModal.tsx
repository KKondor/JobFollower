import { useState, useEffect } from "react";
import type { JobApplicationDto, CreateJobDto, JobPatchDto } from "../types/job";
import { StatusState } from "../types/job";
import styles from "./JobFormModal.module.css";

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
    const [error, setError] = useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

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
        setError(null)
        setIsSubmitting(true);
        try {
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
        catch (err:any){
            const errors = err.response?.data?.errors;
            if (errors) {
                const firstMessage = Object.values(errors)[0] as string[];
                setError(firstMessage[0]);
            } else {
                setError("Something went wrong. Please try again.");
            }
        }
        finally {
            setIsSubmitting(false);
        }
    }

    async function handleDelete() {
        if (!existingJob) return;
        if (!confirm(`Delete "${existingJob.jobName}"? This can't be undone.`)) return;
        await onDelete(existingJob.jobId);
        onClose();
    }

    return (
        <div className={styles.overlay} onClick={onClose}>
            <div className={styles.modal} onClick={(e) => e.stopPropagation()}>
                <button type="button" onClick={onClose} className={styles.closeButton}>
                    ×
                </button>
                <h2 className={styles.title}>{existingJob ? "Edit Job" : "New Job"}</h2>
                <form onSubmit={handleSubmit} className={styles.form}>
                    {error && <p className={styles.error}>{error}</p>}
                    <input
                        type="text"
                        placeholder="Job title"
                        value={jobName}
                        onChange={(e) => setJobName(e.target.value)}
                        className={styles.input}
                        required
                    />
                    <input
                        type="text"
                        placeholder="Description (optional)"
                        value={jobDescription}
                        onChange={(e) => setJobDescription(e.target.value)}
                        className={styles.input}
                    />
                    <select value={status} onChange={(e) => setStatus(e.target.value)} className={styles.select}>
                        {Object.values(StatusState).map((s) => (
                            <option key={s} value={s}>{s}</option>
                        ))}
                    </select>
                    <div className={styles.actions}>
                        <button type="submit" className={styles.submitButton} disabled={isSubmitting}>
                            {isSubmitting ? "Saving..." : existingJob ? "Save" : "Create"}
                        </button>
                        {existingJob && (
                            <button type="button" onClick={handleDelete} className={styles.deleteButton}>
                                Delete
                            </button>
                        )}
                    </div>
                </form>
            </div>
        </div>
    );
}