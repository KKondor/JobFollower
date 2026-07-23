import { useDraggable } from "@dnd-kit/core";
import type { JobApplicationDto } from "../types/job";
import { formatDate } from "../utils/formatDate";
import styles from "./JobCard.module.css"

interface JobCardProps {
    job: JobApplicationDto;
    onClick: () => void;
}

export default function JobCard({ job, onClick }: JobCardProps) {
    const { attributes, listeners, setNodeRef, isDragging } = useDraggable({ id: job.jobId });

    //const style = transform
    //    ? { transform: `translate(${transform.x}px, ${transform.y}px)` }
    //    : undefined;

    return (
        <div
            ref={setNodeRef}
            onClick={onClick}
            className={styles.card}
            style={{ opacity: isDragging ? 0.4 : 1 }}
            {...listeners}
            {...attributes}
        >
            <div className={styles.jobName}>{job.jobName}</div>
            {job.jobDescription && (
                <p className={styles.jobDescription}>{job.jobDescription}</p>
            )}

            {job.appliedDate && (
                <p className={styles.jobDate}>{formatDate(job.appliedDate)}</p>
            )}
        </div>
    );
}