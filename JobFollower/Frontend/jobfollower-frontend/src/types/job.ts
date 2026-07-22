export const StatusState = {
    NotApplied: "NotApplied",
    Applied: "Applied",
    Rejected: "Rejected",
    Ghosted: "Ghosted",
    Interviewed: "Interviewed",
    Accepted: "Accepted",
} as const;

export type StatusState = (typeof StatusState)[keyof typeof StatusState];

export interface JobApplicationDto {
    jobId: number;
    jobName: string;
    jobDescription: string | null;
    status: StatusState;
    appliedDate: string;
}

export interface JobPatchDto {
    jobName?: string;
    jobDescription?: string;
    status?: StatusState;
    appliedDate?: string;
}

export interface CreateJobDto {
    jobName: string;
    jobDescription: string | null;
    status: StatusState;
    appliedDate: string;
}