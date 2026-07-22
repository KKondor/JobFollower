import { createContext } from "react";
import type { UserDto } from "../types/user";

export interface AuthContextType {
    user: UserDto | null;
    isLoading: boolean;
    login: (email: string, password: string) => Promise<void>;
    logout: () => Promise<void>;
    register: (name: string, email: string, password: string) => Promise<void>;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);