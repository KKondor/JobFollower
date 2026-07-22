import { useDraggable } from "@dnd-kit/core";
import type { JobApplicationDto } from "../types/job";

interface JobCardProps {
    job: JobApplicationDto;
    onClick: () => void;
}

export default function JobCard({ job, onClick }: JobCardProps) {
    const { attributes, listeners, setNodeRef, transform } = useDraggable({ id: job.jobId });

    const style = transform
        ? { transform: `translate(${transform.x}px, ${transform.y}px)` }
        : undefined;

    return (
        <div
            ref={setNodeRef}
            onClick={onClick}
            style={{
                border: "1px solid #999",
                padding: "0.5rem",
                marginBottom: "0.5rem",
                ...style,
            }}
            {...listeners}
            {...attributes}
        >
            <strong>{job.jobName}</strong>
            {job.jobDescription && <p>{job.jobDescription}</p>}
            {job.appliedDate} && <p>{job.appliedDate}</p>
        </div>
    );
}