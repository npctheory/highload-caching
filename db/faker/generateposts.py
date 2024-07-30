from faker import Faker
from uuid import uuid4
import random

fake = Faker()

def extract_user_ids(filename):
    """Extract user IDs from an SQL file."""
    user_ids = []
    with open(filename, 'r') as f:
        for line in f:
            if line.startswith("INSERT INTO users"):
                # Extract the user_id from the VALUES clause
                parts = line.split("VALUES (")[1]
                user_id = parts.split(",")[0].strip("('")
                user_ids.append(user_id)
    return user_ids

def generate_posts_sql(user_ids, num_posts_per_user, output_filename, batch_size=1000):
    """Generate SQL insert statements for posts in bulk."""
    with open(output_filename, 'w') as f:
        # Write the initial database connection line
        f.write("\\c highload_social;\n\n")

        insert_statements = []

        for user_id in user_ids:
            for _ in range(num_posts_per_user):
                text = fake.text(max_nb_chars=280)
                created_at = fake.date_time_this_year()  # Generate a random timestamp for the current year
                created_at_str = created_at.strftime('%Y-%m-%d %H:%M:%S')
                
                # Generate a UUID for the post id
                post_id = str(uuid4())

                insert_statements.append(f"('{post_id}', '{text}', '{user_id}', '{created_at_str}')")

                # Write in bulk if batch size is reached
                if len(insert_statements) >= batch_size:
                    f.write("INSERT INTO posts (id, text, author_user_id, created_at) VALUES\n")
                    f.write(",\n".join(insert_statements) + ";\n")
                    insert_statements = []

        # Write remaining statements if any
        if insert_statements:
            f.write("INSERT INTO posts (id, text, author_user_id, created_at) VALUES\n")
            f.write(",\n".join(insert_statements) + ";\n")

if __name__ == "__main__":
    user_ids = extract_user_ids('db/initdb/init001users.sql')
    generate_posts_sql(user_ids, 20, 'db/initdb/init004posts.sql')
