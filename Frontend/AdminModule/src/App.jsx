import { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [users, setUsers] = useState();

    useEffect(() => {
        populateUsersData();
    }, []);

    const contents = users === undefined
        ? <p><em>Loading...</em></p>
        : <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Phone</th>
                </tr>
            </thead>
            <tbody>
                {users.map(user =>
                    <tr key={user.id}>
                        <td>{user.firstName}</td>
                        <td>{user.email}</td>
                        <td>{user.phoneNumber}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tabelLabel">Users</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );
    
    async function populateUsersData() {
        const response = await fetch('../api/users');
        const data = await response.json();
        setUsers(data);
    }
}

export default App;