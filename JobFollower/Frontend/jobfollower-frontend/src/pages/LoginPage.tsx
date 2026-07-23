import { useState } from "react";
import {Link, useNavigate} from "react-router-dom";
import { useAuth } from "../auth/useAuth";
import styles from "./AuthForm.module.css"
import { useDelayedFlag } from "../utils/useDelayedFlag";

export default function LoginPage() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();
    const showColdStartMessage = useDelayedFlag(isSubmitting, 3000);

    async function handleSubmit(e: React.SubmitEvent<HTMLFormElement>) {
        e.preventDefault();
        setError(null);
        setIsSubmitting(true);
        try {
            await login(email, password);
            navigate("/board");
        } catch(err:any) {
            if (err.response?.status === 401) {
                setError("Invalid email or password.");
            } else if (!err.response) {
                setError("Couldn't reach the server. Please check your connection and try again.");
            } else {
                setError("Something went wrong. Please try again.");
            }
        } finally {
            setIsSubmitting(false);
        }
    }

    return (
        <div className={styles.container}>
            <div className={styles.card}>
                <h1 className={styles.title}>Login</h1>
                <form onSubmit={handleSubmit} className={styles.form}>
                    {error && <p className={styles.error}>{error}</p>}
                    <input
                        type="email"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        className={styles.input}
                        required
                    />
                    <input
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        className={styles.input}
                        required
                    />
                    <button type="submit" className={styles.submitButton} disabled={isSubmitting}>
                        {isSubmitting ? "Logging in..." : "Log In"}
                    </button>
                    {showColdStartMessage && (
                        <p className={styles.coldStartMessage}>
                            The server may be waking up from sleep — this can take a moment.
                        </p>
                    )}
                </form>
                <p className={styles.footer}>
                    Don't have an account? <Link to="/register">Register</Link>
                </p>
            </div>
        </div>
    );
}