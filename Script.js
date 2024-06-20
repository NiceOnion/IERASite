import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
  vus: 10,
  duration: '30s',
  cloud: {
    // Project: Default project
    projectID: 3701879,
    // Test runs with the same name groups test runs together.
    name: 'Test (19/06/2024-10:29:18)'
  }
};

export default function () {
    // Define the URL of the microservice endpoint
    let url = 'http://127.0.0.1:50365/';
    
    // Make a GET request
    let response = http.get(url);

    // Check if the response status is 200
    let success = check(response, {
        'status is 200': (r) => r.status === 200,
    });

    // Update custom metrics
    errorRate.add(!success);
    requestCount.add(1);

    // Simulate some user "think time"
    sleep(1);
}