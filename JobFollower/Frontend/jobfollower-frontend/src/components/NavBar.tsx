import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";
import styles from "./Navbar.module.css"

export default function NavBar() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    async function handleLogout() {
        await logout();
        navigate("/login");
    }

    return (
        <nav className={styles.navbar}>
            <Link to="/" className={styles.brand}>JobFollower</Link>
            {user ? (
                <div>
                    <span className={styles.userName}>{user.name}</span>
                    <button className={styles.logoutButton} onClick={handleLogout}>Logout</button>
                </div>
            ) : (
                <div className={styles.links}>
                    <Link className="navbar-link" to="/login" style={{ marginRight: "1rem" }}>Login</Link>
                    <Link className="navbar-link" to="/register">Register</Link>
                </div>
            )}
        </nav>
    );
}