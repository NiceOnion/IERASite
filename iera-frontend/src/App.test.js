import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';

test('renders homepage when path is /', () => {
  render(<App />);
  const homepageElement = screen.getByText(/Home Page/i);
  expect(homepageElement).toBeInTheDocument();
});

test('renders other page when path is /Aankondigingen', () => {
  render(<App />);
  const otherPageElement = screen.getByText(/Aankondigingen/i);
  expect(otherPageElement).toBeInTheDocument();
});
