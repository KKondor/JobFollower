import { useState } from "react";
import {Link, useNavigate} from "react-router-dom";
import { useAuth } from "../auth/useAuth";

export default function RegisterPage() {
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);
    const { register } = useAuth();
    const navigate = useNavigate();

    async function handleSubmit(e: React.SubmitEvent<HTMLFormElement>) {
        e.preventDefault();
        setError(null);
        try {
            await register(name, email, password);
            navigate("/board");
        } catch {
            setError("Invalid input. Please try again.");
        }
    }

    return (
        <>
        <form onSubmit={handleSubmit}>
            <h1>Register</h1>
            {error && <p style={{ color: "red" }}>{error}</p>}
            <input
                type="text"
                placeholder="name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                required
            />
            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
            />
            <button type="submit">Register</button>
        </form>
            <p>Already have an account? <Link to="/login">Login</Link></p>
        </>
    );
}