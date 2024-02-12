import "../Styles/SigninPage.css"
import { useState } from 'react';
import { Link } from 'react-router-dom';

const Register = () => {
    const [username, setUsername] = useState();
    const [password, setPassword] = useState();
    const [email, setEmail] = useState();
    const [telephone, setTelephone] = useState();

    const handleSubmit = (e) => {
        e.preventDefault();
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
                    required
                    value={telephone}
                    onChange={(e) => setTelephone(e.target.value)}
                />
            </div>

            <button className="button">Register</button>
        </form>
    );
}

export default Register;