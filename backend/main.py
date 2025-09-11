from core.runtime import Runtime
import logging

def main():
    """メイン関数"""
    runtime = Runtime()
    runtime.run()

if __name__ == "__main__":
    logging.basicConfig(level=logging.WARNING)
    main()