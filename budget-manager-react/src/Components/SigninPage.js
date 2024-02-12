import "../Styles/SigninPage.css"
import logo from '../Assets/logo.svg';
import { useState } from 'react';
import { Link } from 'react-router-dom';

const SigninPage = () => {
    const [username, setUsername] = useState();
    const [password, setPassword] = useState();

    const handleSubmit = (e) => {
        e.preventDefault();
    }

    return (
        <div className="body-login">
            <div classNamme="left-login">
                {/* <h1>Budget Manager</h1>
                <img src={logo} className="App-logo" alt="logo" /> */}
            </div>

            <div className="center-login">
                <div className="card-login">
                    <div className="user-links">
                        <div className="user-link-home">
                            {/* <Link to="/">Sign In</Link> */}
                            <a to="/">Sign In</a>
                        </div>

                        <div className="user-link-cad">
                            {/* <Link to="/cadastro">Sign up</Link> */}
                            <a to="/cadastro">Sign up</a>
                        </div>
                    </div>
                    <h1>SIGN IN</h1>

                    <form className="login-form" onSubmit={handleSubmit}>
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
                        <a href="/recover">Forgot your password?</a>

                        <button className="button">LOGIN</button>
                    </form>
                </div>
            </div>
        </div>
    );
}

export default SigninPage;