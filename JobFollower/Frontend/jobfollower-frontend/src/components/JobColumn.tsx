import { useDroppable } from "@dnd-kit/core";
import type { JobApplicationDto } from "../types/job";
import JobCard from "./JobCard";

interface JobColumnProps {
    status: string;
    jobs: JobApplicationDto[];
    onCardClick: (job: JobApplicationDto) => void;
}

export default function JobColumn({ status, jobs, onCardClick }: JobColumnProps) {
    const { setNodeRef, isOver } = useDroppable({ id: status });

    return (
        <div
            ref={setNodeRef}
            style={{
                flex: 1,
                border: "1px solid #ccc",
                padding: "0.5rem",
                backgroundColor: isOver ? "#eef" : undefined,
            }}
        >
            <h2>{status}</h2>
            {jobs.map((job) => (
                <JobCard key={job.jobId} job={job} onClick={() => onCardClick(job)} />
            ))}
        </div>
    );
}