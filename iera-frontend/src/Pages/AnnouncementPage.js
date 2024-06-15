import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import APIAddress from '../Data/APIAddress';

function AnnouncementDetails() {
  const [announcement, setAnnouncement] = useState(null);
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [userLoading, setUserLoading] = useState(true);
  const { id } = useParams();

  useEffect(() => {
    fetch(APIAddress + `/Announcement/${id}`)
      .then(response => {
        if (!response.ok) {
          throw new Error('Failed to fetch announcement');
        }
        return response.json();
      })
      .then(data => {
        console.log('Announcement data:', data); // Log the announcement data
        setAnnouncement(data);
        setLoading(false);
        if (data.userID) { // Use the correct key here
          fetchUser(data.userID); // Fetch user data based on userID from announcement
        } else {
          console.error('userID is undefined in announcement data');
        }
      })
      .catch(error => console.error('Error fetching announcement:', error));
  }, [id]);

  const fetchUser = (userId) => {
    if (!userId) {
      console.error('userID is undefined');
      return;
    }

    fetch(APIAddress + `/User/${userId}`)
      .then(response => {
        if (!response.ok) {
          throw new Error('Failed to fetch user');
        }
        return response.json();
      })
      .then(data => {
        console.log('User data:', data); // Log the user data
        setUser(data);
        setUserLoading(false);
      })
      .catch(error => console.error('Error fetching user:', error));
  };

  return (
    <div>
      {loading ? (
        <p>Loading announcement...</p>
      ) : (
        <div>
          <h2>{announcement.title}</h2>
          <p>{announcement.body}</p> {/* Use body instead of content */}
          {userLoading ? (
            <p>Loading user info...</p>
          ) : (
            <div>
              <h3>Posted by: {user.name}</h3>
              <p>Email: {user.email}</p>
              {/* Add more user details as needed */}
            </div>
          )}
        </div>
      )}
    </div>
  );
}

export default AnnouncementDetails;
