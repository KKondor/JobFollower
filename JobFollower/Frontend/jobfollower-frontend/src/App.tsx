import { Routes, Route } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import BoardPage from "./pages/BoardPage";
import ProtectedRoute from "./auth/ProtectedRoute";
import NavBar from "./components/NavBar.tsx";
import RootRedirect from "./components/RootRedirect.tsx";

function App() {
  return (
      <>
      <NavBar />
      <Routes>
          <Route path="/" element={<RootRedirect />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route
            path="/board"
            element={
              <ProtectedRoute>
                <BoardPage />
              </ProtectedRoute>
            }
        />
      </Routes>
      </>
  );
}

export default App;