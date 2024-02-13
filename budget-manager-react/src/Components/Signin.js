import "../Styles/SigninPage.css"
import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { ToastContainer, toast, Bounce } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const Signin = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [isPending, setIsPending] = useState(false);
    const [data, setData] = useState(null);
    const [error, setError] = useState(false);

    const navigate = useNavigate();

    const notifyError = (erroMessage) => toast.error(erroMessage, {
        transition: Bounce,
    });
    const notifySuccess = (successMessage) => toast.success(successMessage, {
        transition: Bounce,
    });

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsPending(true);
        const loginCredentials = { UsernameOrEmail: username, Password: password };

        fetch('https://localhost:7152/api/Auth/login', {
            method: 'POST',
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(loginCredentials)
        }).then(res => {
            if (!res.ok) {
                return res.text().then(err => {
                    throw Error(err);
                });
            }
            return res.json()
        }).then(data => {
            setData(data)
            setIsPending(false);
            localStorage.setItem('@username', JSON.stringify(data.user.username));
            localStorage.setItem('@status', JSON.stringify(data.user.status));
            localStorage.setItem('@token', JSON.stringify(data.token));
            notifySuccess("Login Successfull!")
            // Redirect to Dashboard
            navigate('/dashboard');
            window.location.reload();
        }).catch(err => {
            setError(err.message);
            setIsPending(false);

            notifyError(err.message);
        })
    }

    return (
        <form className="login-form" onSubmit={handleSubmit}>
            <h1>LOGIN</h1>
            <div className="form-group">
                <label htmlFor="">Username or email</label>
                <input
                    className="form-field"
                    type="text"
                    required
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />
            </div>

            <div className="form-group">
                <label htmlFor="">Password</label>
                <input
                    className="form-field"
                    type="password"
                    required
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
            </div>
            <div className="login-link">
                <Link to="/recover">Forgot your password?</Link>
            </div>

            {!isPending && <button className="button">LOGIN</button>}
            {isPending && <button disabled className="button">Loading...</button>}
            <ToastContainer
                position="top-center"
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
                theme="light" />
        </form>
    );
}

export default Signin;