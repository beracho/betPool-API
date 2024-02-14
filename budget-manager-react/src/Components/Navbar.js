import { useNavigate } from "react-router-dom";

const Navbar = () => {
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
            <button onClick={(e) => handleClick(e)}>Sign out</button>
        </div>
    );
}

export default Navbar;