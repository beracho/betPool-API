import "../Styles/SigninPage.css"
import 'react-toastify/dist/ReactToastify.css';
import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { ToastContainer, toast, Bounce } from 'react-toastify';
import { fetchData } from '../Services/UseFetch';

const Signin = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [isPending, setIsPending] = useState(false);

    const navigate = useNavigate();

    const notifyError = (erroMessage) => toast.error(erroMessage, {
        transition: Bounce,
    });
    const notifySuccess = (successMessage) => toast.success(successMessage, {
        transition: Bounce,
    });

    const isJSON = (str) => {
        try {
            return (JSON.parse(str) && !!str);
        } catch (e) {
            return false;
        }
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsPending(true);
        const loginCredentials = { UsernameOrEmail: username, Password: password };

        const url = '/Auth/login';
        const options = {
            method: 'POST',
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(loginCredentials)
        };

        const { data: dataFetch, error: errorFetch } = await fetchData(url, options);

        if (!errorFetch) {
            setIsPending(false);
            dataFetch.json().then(response => {
                console.log("response");
                console.log(response);
                console.log(response.user.username);
                localStorage.setItem('@username', response.user.username);
                localStorage.setItem('@status', response.user.status);
                localStorage.setItem('@token', response.token);
                // Redirect to Dashboard
                navigate('/dashboard');
                window.location.reload();
            });
            notifySuccess("Login Successfull!")
        } else {
            setIsPending(false);
            if (isJSON(errorFetch)) {
                var errObject = JSON.parse(errorFetch);
                if (errObject.errors) {
                    for (const [key, value] of Object.entries(errObject.errors)) {
                        value.forEach(errorMessage => {
                            notifyError(errorMessage);
                        });
                    }
                }
            } else {
                notifyError(errorFetch);
            }
        }
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