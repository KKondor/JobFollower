import styles from "./LoadingScreen.module.css";
import type {ReactNode} from "react";

interface LoadingScreenProps {
    message?: string;
    children?: ReactNode;
}

export default function LoadingScreen({ message = "Loading...", children }: LoadingScreenProps) {
    return (
        <div className={styles.container}>
            <div className={styles.spinner} />
            <p className={styles.message}>{message}</p>
            {children}
        </div>
    );
}