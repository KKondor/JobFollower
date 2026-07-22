import axios from "axios";
import { getAccessToken, setAccessToken } from "./tokenStore";
import type {LoginResponseDto} from "../types/user.ts";

const api = axios.create({
    baseURL: import.meta.env.VITE_API_URL,
    withCredentials: true,
});

api.interceptors.request.use((config) => {
    const token = getAccessToken();
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

api.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        if (
            error.response?.status === 401 &&
            !originalRequest._retry &&
            !originalRequest.url?.includes("/auth/refresh")
        ) {
            originalRequest._retry = true;
            try {
                const refreshResponse = await axios.post<LoginResponseDto>(
                    `${import.meta.env.VITE_API_URL}/auth/refresh`,
                    {},
                    { withCredentials: true }
                );
                const newToken = refreshResponse.data.accessToken;
                setAccessToken(newToken);
                originalRequest.headers.Authorization = `Bearer ${newToken}`;
                return api(originalRequest);
            } catch (refreshError) {
                setAccessToken(null);
                // TODO: redirect to login once we build routing
                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error);
    }
);

export default api;