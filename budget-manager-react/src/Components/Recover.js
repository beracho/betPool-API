import "../Styles/SigninPage.css"
import { useState } from 'react';
import { Link } from 'react-router-dom';

const Recover = () => {
    const [username, setUsername] = useState();
    const [password, setPassword] = useState();

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
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />
            </div>
            <div className="login-link">
                <a href="/">Back to login</a></div>

            <button className="button">SEND EMAIL</button>
        </form>
    );
}

export default Recover;