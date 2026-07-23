import { useState, useEffect, useRef } from "react";
import type { ReactNode } from "react";
import { AuthContext } from "./AuthContext";
import type { UserDto, LoginResponseDto } from "../types/user";
import { setAccessToken } from "../api/tokenStore";
import api from "../api/axiosInstance";

export function AuthProvider({ children }: { children: ReactNode }) {
    const [user, setUser] = useState<UserDto | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const hasAttemptedRestore = useRef(false);

    async function login(email: string, password: string) {
        const response = await api.post<LoginResponseDto>("/auth/login", { email, password });
        setAccessToken(response.data.accessToken);
        setUser(response.data.user);
    }

    async function logout() {
        try {
            await api.post("/auth/logout");
        } catch {
            // Refresh cookie likely wasn't sent (e.g. Firefox/Safari blocking third-party
            // cookies) or was already invalid - either way, there's nothing left to revoke
            // server-side. Proceed with clearing local state regardless.
        }
        finally {
                setAccessToken(null);
                setUser(null);
            }
    }

    async function register(name: string, email: string, password: string) {
        await api.post("/auth/register", { name, email, password });
        await login(email, password);
    }

    useEffect(() => {
        if (hasAttemptedRestore.current) return;
        hasAttemptedRestore.current = true;

        async function tryRestoreSession() {
            try {
                const response = await api.post<LoginResponseDto>("/auth/refresh");
                setAccessToken(response.data.accessToken);
                setUser(response.data.user);
            } catch {
                setAccessToken(null);
                setUser(null);
            } finally {
                setIsLoading(false);
            }
        }

        tryRestoreSession();
    }, []);

    return (
        <AuthContext.Provider value={{ user, isLoading, login, logout, register }}>
            {children}
        </AuthContext.Provider>
    );
}