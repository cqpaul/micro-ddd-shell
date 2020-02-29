import React, { Component } from 'react'
import { Map, Marker, NavigationControl } from 'react-bmap'
import api from '../../api'
import './index.css'

export default class MapChart extends Component {
  constructor(props) {
    super(props)

    this.state = {
      cqLocation: { lng: 106.55, lat: 29.57 },
      mapBody: (
        <div>
          <span className="small-title">房价地图</span>
          <Map
            center={{ lng: 106.55, lat: 29.57 }}
            zoom="11"
            mapStyle={{ style: 'midnight' }}
            style={{ height: '527px', paddingTop: '10px' }}
          >
            <NavigationControl />
          </Map>
        </div>
      )
    }
  }

  buildMap(villageName) {
    let getUrl = '/ReportingService/Reporting/GetLocationInfos'
    if (villageName !== 'undefined' && villageName !== undefined) {
      getUrl = `/ReportingService/Reporting/GetLocationInfos?villageName=${villageName}`
    }
    api
      .callApi('GET', getUrl)
      .then(response => {
        let markerList = response.map((marker, index) => {
          return (
            <Marker
              key={index}
              position={{ lng: marker.lng, lat: marker.lat }}
              title={marker.villageName}
            />
          )
        })
        let newBody = (
          <div>
            <span className="small-title">房价地图</span>
            <Map
              center={{ lng: 106.55, lat: 29.57 }}
              zoom="11"
              mapStyle={{ style: 'midnight' }}
              style={{ height: '527px', paddingTop: '10px' }}
            >
              {markerList}
              <NavigationControl />
            </Map>
          </div>
        )
        this.setState({ mapBody: newBody })
      })
      .then(() => {
        this.props.finishedFetching()
      })
  }

  render() {
    return <div>{this.state.mapBody}</div>
  }
}
