import { useEffect } from "react";
import api from "./api/axiosInstance";

function App() {
  useEffect(() => {
    api.get("/jobs")
        .then((res) => console.log("Success:", res.data))
        .catch((err) => console.log("Error:", err.response?.status, err.message));
  }, []);

  return <div>Testing API connection...</div>;
}

export default App;