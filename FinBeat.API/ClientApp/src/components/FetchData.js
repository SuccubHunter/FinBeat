import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = {
      items: [],
      page: 1,
      pageSize: 10,
      totalCount: 0,
      loading: false,
      jsonInput: '[{"code":"1","value":"value1"},{"code":"2","value":"value3"},{"code":"3","value":"value2"}]',
      error: null,
      uploading: false,
    };
  }

  componentDidMount() {
    this.fetchItems(this.state.page);
  }

  fetchItems(page) {
    this.setState({ loading: true, error: null });
    fetch(`https://localhost:7097/api/items`)
      .then(res => {
        if (!res.ok) throw new Error('Ошибка загрузки');
        return res.json();
      })
      .then(data => {
        const items = Array.isArray(data) ? data : [];
        this.setState({
          items,
          totalCount: items.length,
          loading: false,
          page,
        });
      })
      .catch(err => this.setState({ error: err.message, loading: false }));
  }

  handleJsonChange = (e) => {
    this.setState({ jsonInput: e.target.value });
  };

  handleUpload = () => {
    this.setState({ uploading: true, error: null });

    let parsed;
    try {
      parsed = JSON.parse(this.state.jsonInput);
      if (!Array.isArray(parsed)) throw new Error('Должен быть массив объектов');
    } catch (ex) {
      this.setState({ error: 'Неверный формат JSON', uploading: false });
      return;
    }

    fetch('https://localhost:7097/api/items', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(parsed),
    })
      .then(res => {
        if (!res.ok) throw new Error('Ошибка при загрузке данных');
        return res.text(); // POST может вернуть 204 No Content
      })
      .then(() => {
        this.setState({ uploading: false, jsonInput: '' });
        this.fetchItems(1);
      })
      .catch(err => this.setState({ error: err.message, uploading: false }));
  };

  goPrev = () => {
    if (this.state.page > 1) this.fetchItems(this.state.page - 1);
  };

  goNext = () => {
    const totalPages = Math.ceil(this.state.totalCount / this.state.pageSize);
    if (this.state.page < totalPages) this.fetchItems(this.state.page + 1);
  };

  render() {
    const { items, page, pageSize, totalCount, loading, jsonInput, error, uploading } = this.state;
    const totalPages = Math.ceil(totalCount / pageSize);

    return (
      <div>
        <h2>Upload JSON Data</h2>
        <textarea
          rows={6}
          style={{ width: '100%', fontFamily: 'monospace' }}
          value={jsonInput}
          onChange={this.handleJsonChange}
          disabled={uploading}
        />
        <br />
        <button
          className="btn btn-success"
          onClick={this.handleUpload}
          disabled={uploading}
        >
          {uploading ? 'Uploading...' : 'Upload'}
        </button>
        {error && <p style={{ color: 'red' }}>{error}</p>}

        <hr />

        <h2>Items List</h2>
        {loading && <p>Loading...</p>}

        {!loading && (
          <>
            <table className="table table-bordered" style={{ width: '100%' }}>
              <thead>
                <tr>
                  <th>#</th>
                  <th>Code</th>
                  <th>Value</th>
                </tr>
              </thead>
              <tbody>
                {(!items || items.length === 0) ? (
                  <tr>
                    <td colSpan="3">No data</td>
                  </tr>
                ) : (
                  items.map((item, idx) => (
                    <tr key={item.id || idx}>
                      <td>{(page - 1) * pageSize + idx + 1}</td>
                      <td>{item.code}</td>
                      <td>{item.value}</td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>

            <div style={{ marginTop: 10 }}>
              <button
                className="btn btn-primary me-2"
                onClick={this.goPrev}
                disabled={page === 1}
              >
                Prev
              </button>
              <span>
                Page {page} of {totalPages}
              </span>
              <button
                className="btn btn-primary ms-2"
                onClick={this.goNext}
                disabled={page === totalPages}
              >
                Next
              </button>
            </div>
          </>
        )}
      </div>
    );
  }
}
