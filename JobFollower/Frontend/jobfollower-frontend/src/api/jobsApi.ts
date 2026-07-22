import api from "./axiosInstance";
import type { JobApplicationDto, CreateJobDto, JobPatchDto } from "../types/job";

export async function getJobs(): Promise<JobApplicationDto[]> {
    const response = await api.get<JobApplicationDto[]>("/jobs");
    return response.data;
}

export async function getJobById(id: number): Promise<JobApplicationDto> {
    const response = await api.get<JobApplicationDto>(`/jobs/${id}`);
    return response.data;
}

export async function createJob(job: CreateJobDto): Promise<JobApplicationDto> {
    const response = await api.post<JobApplicationDto>("/jobs", job);
    return response.data;
}

export async function patchJob(id: number, patch: JobPatchDto): Promise<JobApplicationDto> {
    const response = await api.patch<JobApplicationDto>(`/jobs/${id}`, patch);
    return response.data;
}

export async function deleteJob(id: number): Promise<void> {
    await api.delete(`/jobs/${id}`);
}