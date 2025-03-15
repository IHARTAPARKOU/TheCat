Functional Requirements

Fetching the List of Cat Breeds with Pagination:
1.1. Load data from the API page by page (20 items per page).
1.2. When scrolling down, load the next page and store the data in a local cache.

Search and Filtering:
2.1. Implement search by breed name:
2.1.1. Search in the local cache first.
2.1.2. If no data is found, send a request to the API.
2.2. Add filtering by country of origin, allowing selection of one or multiple countries via a dropdown list.

Favorites:
3.1. Implement the ability to add/remove breeds from the "Favorites" list.
3.2. Favorite breeds should always appear at the top of the list and be visually highlighted (e.g., with a star icon).
3.3. The "Favorites" list should persist between application sessions (store it in a file, such as JSON or XML).

Details View:
4.1. When clicking on a breed, display a modal window with detailed information:
4.1.1. Name, description, country of origin, and average size.
4.1.2. Multiple images of the breed.
4.1.3. A link to the Wikipedia article.
4.2. Allow users to edit breed details (except for images) and save changes to the local cache (serialize to a file).

Working with Images:
5.1. When fetching data, save images to disk (e.g., in an "Images" folder next to the application).
5.2. Use locally stored images to minimize network requests.

Styling:
6.1. Use styles and templates (WPF Styles and Templates) for data display.
6.2. Implement a responsive interface that adapts correctly when resizing the window.
