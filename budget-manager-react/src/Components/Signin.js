import "../Styles/SigninPage.css"
import { useState } from 'react';
import { Link } from 'react-router-dom';

const Signin = () => {
    const [username, setUsername] = useState();
    const [password, setPassword] = useState();

    const handleSubmit = (e) => {
        e.preventDefault();
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
                <a href="/recover">Forgot your password?</a>
            </div>

            <button className="button">LOGIN</button>
        </form>
    );
}

export default Signin;