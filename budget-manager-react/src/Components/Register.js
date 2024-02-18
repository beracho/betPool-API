import "../Styles/SigninPage.css"
import 'react-toastify/dist/ReactToastify.css';
import { useState } from 'react';
import { useNavigate } from "react-router-dom";
import { ToastContainer, toast, Bounce } from 'react-toastify';
import validator from 'validator';
import { fetchData } from '../Services/UseFetch';

const Register = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");
    const [telephone, setTelephone] = useState("");

    const [isPending, setIsPending] = useState(false);
    const [data, setData] = useState(null);

    const navigate = useNavigate();

    const [emailError, setEmailError] = useState('');
    const [telephoneError, setTelephoneError] = useState('');

    const handleEmailChange = (e) => {
        e.preventDefault();
        setEmail(e.target.value);

        if (validator.isEmail(e.target.value)) {
            setEmailError('')
        } else {
            setEmailError('Enter valid Email!')
        }
    }

    const handlePhoneChange = (e) => {
        e.preventDefault();
        console.log(e.target.value);
        setTelephone(e.target.value);

        if (validator.isMobilePhone(e.target.value)) {
            setTelephoneError('')
        } else {
            setTelephoneError('Enter valid phone number!')
        }
    }

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

        var fieldsValidated = true;

        if (validator.isEmail(email)) {
            setEmailError('')
        } else {
            setEmailError('Enter valid Email!')
            fieldsValidated = false;
        }
        if (validator.isMobilePhone(telephone)) {
            setTelephoneError('')
        } else {
            setTelephoneError('Enter valid phone number!')
            fieldsValidated = false;
        }

        if (fieldsValidated) {
            const registerObject = {
                username: username,
                password: password,
                email: email,
                telephone: telephone
            };

            const url = '/Auth/Register';
            const options = {
                method: 'POST',
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(registerObject)
            };

            const { data: dataFetch, error: errorFetch } = await fetchData(url, options);

            if (!errorFetch) {
                setData(dataFetch)
                notifySuccess("Account registered successfully!")

                navigate('/');
            } else {
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
        setIsPending(false);
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
                    onChange={(e) => handleEmailChange(e)}
                />
                <span style={{ color: 'red' }}>{emailError}</span>
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
                    onChange={(e) => handlePhoneChange(e)}
                />
                <span style={{ color: 'red' }}>{telephoneError}</span>
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