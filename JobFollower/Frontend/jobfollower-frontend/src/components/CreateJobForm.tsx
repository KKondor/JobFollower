import { useState } from "react";
import { createJob } from "../api/jobsApi"
import type { JobApplicationDto, CreateJobDto } from "../types/job";
import { StatusState } from "../types/job";

interface CreateJobFormProps {
    onJobCreated: (job: JobApplicationDto) => void;
}

export default function CreateJobForm({ onJobCreated }: CreateJobFormProps) {
    const [jobName, setJobName] = useState("");
    const [jobDescription, setJobDescription] = useState("");
    const [applyDate, setApplyDate] = useState("");
    const [error, setError] = useState<string | null>(null);

    async function handleSubmit(e: React.SubmitEvent<HTMLFormElement>) {
        e.preventDefault();
        setError(null);

        try {
            const newJob: CreateJobDto = {
                jobName,
                jobDescription: jobDescription || null,
                status: StatusState.NotApplied,
                appliedDate: applyDate ? new Date(applyDate).toISOString() : new Date().toISOString(),
            };
            const created = await createJob(newJob);
            onJobCreated(created);
            setJobName("");
            setJobDescription("");
            setApplyDate("");
        } catch {
            setError("Failed to create job application.");
        }
    }
    return (
        <form className="create-job-form" onSubmit={handleSubmit}>
            {error && <p className="create-job-form-error">{error}</p>}
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
            <input
                type="date"
                value={applyDate}
                onChange={(e) => setApplyDate(e.target.value)}
            />
            <button type="submit">Add Job</button>
        </form>
    );
}