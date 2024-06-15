import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import APIAddress from '../Data/APIAddress';

function AnnouncementList() {
  const [announcements, setAnnouncements] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch(APIAddress + "/Announcement/All")
      .then(response => response.json())
      .then(data => {
        setAnnouncements(data);
        setLoading(false);
        console.log(data); // Log the fetched data here
      })
      .catch(error => console.error('Error fetching announcements:', error));
  }, []);

  return (
    <div>
      <h1>Announcements</h1>
      <Link to="/announcement/create">Create Announcement</Link>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <ul>
          {announcements.map(announcement => (
            <li key={announcement.id}>
              <Link to={`/announcement/${announcement.id}`}>
                <h3>{announcement.title}</h3>
              </Link>
              <p>{announcement.content}</p>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default AnnouncementList;
