import hashlib
from faker import Faker

fake = Faker()

def hash_password(password):
    """Hash the password using SHA-256."""
    return hashlib.sha256(password.encode()).hexdigest()

def generate_unique_id(existing_ids, index, length=255):
    """Generate a unique alphanumeric ID of a specified length."""
    while True:
        # Generate two random words in PascalCase and append an index
        word1 = fake.word().capitalize()
        word2 = fake.word().capitalize()
        id = f"{word1}{word2}{index}"
        if len(id) > length:
            id = id[:length]  # Truncate to fit the length
        if id not in existing_ids:
            existing_ids.add(id)
            return id

def generate_sql_users(filename, num_records, user_ids):
    """Generate SQL file with fake user records for the users table."""
    with open(filename, 'w') as f:
        # Write the initial database connection line
        f.write("\\c highload_social;\n\n")

        # Generate user data
        password_hash = hash_password("password")
        index = 1
        for _ in range(num_records):
            user_id = generate_unique_id(user_ids, index)
            f.write(f"INSERT INTO users (id, password_hash) VALUES ('{user_id}', '{password_hash}');\n")
            index += 1

def generate_sql_profiles(filename, num_records, user_ids):
    """Generate SQL file with fake user records for the profiles table."""
    with open(filename, 'w') as f:
        # Write the initial database connection line
        f.write("\\c highload_social;\n\n")

        # Generate profile data
        for index, user_id in enumerate(user_ids):
            first_name = fake.first_name()
            second_name = fake.last_name()
            birthdate = fake.date_of_birth(minimum_age=18, maximum_age=90).strftime('%Y-%m-%d')
            biography = fake.text(max_nb_chars=255)
            city = fake.city()
            f.write(f"INSERT INTO profiles (user_id, first_name, second_name, birthdate, biography, city) VALUES ('{user_id}', '{first_name}', '{second_name}', '{birthdate}', '{biography}', '{city}');\n")

if __name__ == "__main__":
    num_records = 5000
    user_ids = set()
    generate_sql_users('db/initdb/init001users.sql', num_records, user_ids)
    generate_sql_profiles('db/initdb/init002profiles.sql', num_records, user_ids)
