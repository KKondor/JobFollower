import { Navigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";

export default function RootRedirect() {
    const { user, isLoading } = useAuth();
    if (isLoading) return <div>Loading...</div>;
    return <Navigate to={user ? "/board" : "/login"} replace />;
}