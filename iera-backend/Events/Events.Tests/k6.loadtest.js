import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    stages: [
        { duration: '30s', target: 20 }, // ramp-up to 20 users
        { duration: '1m', target: 20 },  // stay at 20 users
        { duration: '10s', target: 0 },  // ramp-down to 0 users
    ],
};

const API_BASE_URL = 'http://your-aspnet-app-url.com/api/events';

export default function () {
    // Perform a GET request
    let getRes = http.get(`${API_BASE_URL}`);
    check(getRes, {
        'GET status was 200': (r) => r.status === 200,
    });

    // Perform a POST request
    let payload = JSON.stringify({
        id: Math.floor(Math.random() * 1000),  // Example id
        name: `Event ${Math.floor(Math.random() * 1000)}`,  // Example name
        // Add other event properties as needed
    });

    let params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    let postRes = http.post(`${API_BASE_URL}`, payload, params);
    check(postRes, {
        'POST status was 200': (r) => r.status === 200,
    });

    sleep(1);
}
