import { Navigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";
import { useDelayedFlag } from "../utils/useDelayedFlag"
import styles from "./RootRedirect.module.css";
import LoadingScreen from "./LoadingScreen";



export default function RootRedirect() {
    const { user, isLoading } = useAuth();
    const showColdStartMessage = useDelayedFlag(isLoading, 3000);

    if (isLoading) {
        return (
            <LoadingScreen>
                {showColdStartMessage && (
                    <p className={styles.coldStartMessage}>
                        The server may be waking up from sleep — this can take up to a minute.
                    </p>
                )}
            </LoadingScreen>
        );
    }

    return <Navigate to={user ? "/board" : "/login"} replace />;
}