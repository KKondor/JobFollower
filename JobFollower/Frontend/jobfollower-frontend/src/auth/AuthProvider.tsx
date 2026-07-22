import { useState, useEffect } from "react";
import type { ReactNode } from "react";
import { AuthContext } from "./AuthContext";
import type { UserDto } from "../types/user";
import { setAccessToken } from "../api/tokenStore";
import api from "../api/axiosInstance";

export function AuthProvider({ children }: { children: ReactNode }) {
    const [user, setUser] = useState<UserDto | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    async function login(email: string, password: string) {
        const response = await api.post<LoginResponseDto>("/auth/login", { email, password });
        setAccessToken(response.data.accessToken);
        setUser(response.data.user);
    }

    async function logout() {
        await api.post("/auth/logout");
        setAccessToken(null);
        setUser(null);
    }

    async function register(name: string, email: string, password: string) {
        await api.post("/auth/register", { name, email, password });
        await login(email, password);
    }

    useEffect(() => {
        setIsLoading(false);
    }, []);

    return (
        <AuthContext.Provider value={{ user, isLoading, login, logout, register }}>
            {children}
        </AuthContext.Provider>
    );
}