import "../Styles/SigninPage.css"
import { useState } from 'react';
import { Link } from 'react-router-dom';

const Signin = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [isPending, setIsPending] = useState(false);
    const [data, setData] = useState(null);
    const [error, setError] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsPending(true);
        const loginCredentials = { UsernameOrEmail: username, Password: password };

        fetch('https://localhost:7152/api/Auth/login', {
            method: 'POST',
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(loginCredentials)
        }).then(res => {
            console.log(res.headers);
            if (!res.ok) {
                return res.text().then(err => {
                    console.log(err);
                    throw Error(err);
                });
            }
            return res.json()
        }).then(data => {
            setData(data)
            console.log(data);
            setIsPending(false);
            // Redirect to Dashboard
        }).catch(err => {
            console.log("error catch", err);
            setError(err.message);
            setIsPending(false);
            //Display error message
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
        </form>
    );
}

export default Signin;