import "../Styles/SigninPage.css"
import { useState } from 'react';
import { Link } from 'react-router-dom';
import { ToastContainer, toast, Bounce } from 'react-toastify';
import validator from 'validator';
import { fetchData } from '../Services/UseFetch';

const Recover = () => {
    const [email, setEmail] = useState("");
    const [emailError, setEmailError] = useState('');
    const [isPending, setIsPending] = useState(false);

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

    const handleEmailChange = (e) => {
        e.preventDefault();
        setEmail(e.target.value);

        if (validator.isEmail(e.target.value)) {
            setEmailError('')
        } else {
            setEmailError('Enter valid Email!')
        }
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsPending(true);
        const recoverPassword = { recoveryEmail: email };

        const url = '/Auth/Recover';
        const options = {
            method: 'PATCH',
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(recoverPassword)
        };

        const { data: dataFetch, error: errorFetch } = await fetchData(url, options);

        if (!errorFetch) {
            setIsPending(false);
            notifySuccess("An email has been send to " + recoverPassword.RecoveryEmail + " with a link to reset your password")
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
                notifyError("Couldn't send the email at the moment, try again later.");
            }
        }
    }

    return (
        <form className="login-form" onSubmit={handleSubmit}>
            <h1>RECOVER PASSWORD</h1>
            <div className="form-group">
                <label htmlFor="">Email</label>
                <input
                    className="form-field"
                    type="text"
                    required
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <span style={{ color: 'red' }}>{emailError}</span>
            </div>
            <div className="login-link">
                <Link to="/login">Back to login</Link>
            </div>

            {!isPending && <button className="button">SEND EMAIL</button>}
            {isPending && <button disabled className="button">SENDING EMAIL...</button>}
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

export default Recover;