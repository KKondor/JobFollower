import type { ReactNode } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "./useAuth";

export default function ProtectedRoute({ children }: { children: ReactNode }) {
    const { user, isLoading } = useAuth();

    if (isLoading) {
        return <div>Loading...</div>; // or a real spinner component later
    }

    if (!user) {
        return <Navigate to="/login" replace />;
    }

    return <>{children}</>;
}