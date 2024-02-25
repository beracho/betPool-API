import "../Styles/SigninPage.css"
import 'react-toastify/dist/ReactToastify.css';
import { useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { ToastContainer, toast, Bounce } from 'react-toastify';
import { fetchData } from '../Services/UseFetch';

const NewPassword = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [comparePassword, setComparePassword] = useState("");
    const [isPending, setIsPending] = useState(false);
    const [passwordError, setPasswordError] = useState("");

    const navigate = useNavigate();
    const params = useParams();

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

    const handleComparePassword = (e) => {
        setComparePassword(e.target.value)
        if (password === e.target.value)
            setPasswordError();
        else
            setPasswordError("Passwords don't match");
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (password === comparePassword) {
            setIsPending(true);

            const setPassword = { NewPassword: password, RecoveryKey: params.code, Email: username };

            const url = '/Auth/ResetPassword';
            const options = {
                method: 'PATCH',
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(setPassword)
            };

            const { data: dataFetch, error: errorFetch } = await fetchData(url, options);

            if (!errorFetch) {
                setIsPending(false);
                notifySuccess("Your password has been reset!")
                navigate('/login');
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
        } else {
            setPasswordError("Passwords don't match");
            notifyError("Passwords don't match");
        }
    }

    return (
        <form className="login-form" onSubmit={handleSubmit}>
            <h1>Reset password</h1>
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
                <label htmlFor="">New password</label>
                <input
                    className="form-field"
                    type="password"
                    required
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
            </div>

            <div className="form-group">
                <label htmlFor="">Retype your password</label>
                <input
                    className="form-field"
                    type="password"
                    required
                    value={comparePassword}
                    onChange={(e) => handleComparePassword(e)}
                />
                <span style={{ color: 'red' }}>{passwordError}</span>
            </div>

            {!isPending && <button className="button">Update Password</button>}
            {isPending && <button disabled className="button">Updating...</button>}
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

export default NewPassword;