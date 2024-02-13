import { useNavigate } from "react-router-dom";

const Dashboard = () => {
    const navigate = useNavigate();

    const handleClick = (e) => {
        e.preventDefault();
        localStorage.removeItem('@username');
        localStorage.removeItem('@status');
        localStorage.removeItem('@token');
        navigate('/login');
        window.location.reload();
    };

    return (
        <div>
            <h1>This is the dashboard</h1>
            <button onClick={(e) => handleClick(e)}>Sign out</button>
        </div>
    );
}

export default Dashboard;