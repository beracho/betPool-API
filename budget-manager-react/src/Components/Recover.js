import "../Styles/SigninPage.css"
import { useState } from 'react';
import { Link } from 'react-router-dom';
import validator from 'validator';

const Recover = () => {
    const [email, setEmail] = useState("");
    const [emailError, setEmailError] = useState('');

    const handleEmailChange = (e) => {
        e.preventDefault();
        setEmail(e.target.value);

        if (validator.isEmail(e.target.value)) {
            setEmailError('')
        } else {
            setEmailError('Enter valid Email!')
        }
    }

    const handleSubmit = (e) => {
        e.preventDefault();
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
                    onChange={(e) => handleEmailChange(e)}
                />
                <span style={{ color: 'red' }}>{emailError}</span>
            </div>
            <div className="login-link">
                <Link to="/login">Back to login</Link>
            </div>

            <button className="button">SEND EMAIL</button>
        </form>
    );
}

export default Recover;