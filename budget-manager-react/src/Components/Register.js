import "../Styles/SigninPage.css"
import { useState } from 'react';
import { useNavigate } from "react-router-dom";
import { ToastContainer, toast, Bounce } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const Register = () => {
    const [username, setUsername] = useState();
    const [password, setPassword] = useState();
    const [email, setEmail] = useState();
    const [telephone, setTelephone] = useState();
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
        const registerObject = {
            username: username,
            password: password,
            email: email,
            telephone: telephone
        };

        fetch('https://localhost:7152/api/Auth/Register', {
            method: 'POST',
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(registerObject)
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
            notifySuccess("Account registered successfully!")

            navigate('/');
        }).catch(err => {
            setIsPending(false);
            if (isJSON(err.message)) {
                setError(err.message);
                var errObject = JSON.parse(err.message);
                if (errObject.errors) {
                    setError(errObject.errors);
                    for (const [key, value] of Object.entries(errObject.errors)) {
                        value.forEach(errorMessage => {
                            notifyError(errorMessage);
                        });
                    }
                }
            } else {
                setError(err.message);
                notifyError(err.message);
            }

        })
    }

    return (
        <form className="login-form" onSubmit={handleSubmit}>
            <h1>REGISTER</h1>
            <div className="form-group">
                <label htmlFor="">Username</label>
                <input
                    className="form-field"
                    type="text"
                    required
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />
            </div>

            <div className="form-group">
                <label htmlFor="">Email</label>
                <input
                    className="form-field"
                    type="text"
                    required
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
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

            <div className="form-group">
                <label htmlFor="">Telephone</label>
                <input
                    className="form-field"
                    type="text"
                    value={telephone}
                    onChange={(e) => setTelephone(e.target.value)}
                />
            </div>

            {!isPending && <button className="button">Register</button>}
            {isPending && <button className="button">Creating user</button>}
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

export default Register;