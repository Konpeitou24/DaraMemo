from flask import Flask
from api.routes import break_routes

app = Flask(__name__)
app.register_blueprint(break_routes.bp)

if __name__ == "__main__":
    app.run(port=5000)
