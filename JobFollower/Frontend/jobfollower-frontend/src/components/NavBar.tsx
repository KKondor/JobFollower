import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";

export default function NavBar() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    async function handleLogout() {
        await logout();
        navigate("/login");
    }

    return (
        <nav style={{ display: "flex", justifyContent: "space-between", padding: "1rem", borderBottom: "1px solid #ccc" }}>
            <Link to="/">JobFollower</Link>
            {user ? (
                <div>
                    <span style={{ marginRight: "1rem" }}>{user.name}</span>
                    <button onClick={handleLogout}>Logout</button>
                </div>
            ) : (
                <div>
                    <Link to="/login" style={{ marginRight: "1rem" }}>Login</Link>
                    <Link to="/register">Register</Link>
                </div>
            )}
        </nav>
    );
}