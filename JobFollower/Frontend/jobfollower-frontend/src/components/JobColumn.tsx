import { useDroppable } from "@dnd-kit/core";
import type { JobApplicationDto } from "../types/job";
import JobCard from "./JobCard";
import styles from "./JobColumn.module.css"

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
            className={`${styles.column} ${isOver ? styles.columnOver : ""}`}
        >
            <h2 className={styles.columnTitle}>{status}</h2>
            {jobs.map((job) => (
                <JobCard key={job.jobId} job={job} onClick={() => onCardClick(job)} />
            ))}
        </div>
    );
}