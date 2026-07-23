import type { ReactNode } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "./useAuth";
import LoadingScreen from "../components/LoadingScreen";

export default function ProtectedRoute({ children }: { children: ReactNode }) {
    const { user, isLoading } = useAuth();

    if (isLoading) return <LoadingScreen />;

    if (!user) {
        return <Navigate to="/login" replace />;
    }

    return <>{children}</>;
}