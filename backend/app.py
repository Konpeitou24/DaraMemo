from flask import Flask, jsonify, request

# Initialize the Flask application
app = Flask(__name__)

if __name__ == "__main__":
    app.run(debug=True)
